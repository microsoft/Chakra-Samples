#include "qjsengine.h"
#include "ChakraCore.h"
#include <QStack>
#include <QMetaObject>
#include <QMetaMethod>
#include <QGenericArgument>
#include <QStack>
#include <stdexcept>
#define EXCEPTION_MAX_LENGTH 255
#define JS_EXPORT_BASE_NAME  "_XY_"
static JsValueRef caller(JsValueRef, bool, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
static JsValueRef set(JsValueRef, bool, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
static JsValueRef get(JsValueRef, bool, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
static bool registerFunction(JsValueRef hostObject, const QByteArray& callbackName, JsNativeFunction callback, void *callbackState);
static bool runMethod(QObject* object, QMetaMethod& method, QVariantList& parameters, QVariant& retour);

namespace chakracorejsengine
{
  class ErrorCode
  {
    bool withException;
    JsErrorCode errCode;
    QString errString;
  public:
    typedef enum {DEBUG, INFO, WARNING, CRITICAL, FATAL} ErrorLevel;
    explicit ErrorCode(bool withException = true):withException(withException), errCode(JsNoError){}
    explicit ErrorCode(JsErrorCode errCode, bool withException = true):withException(withException)
    {
      (*this) = errCode;
    }
    ErrorCode(const ErrorCode& errorCode)
      :withException(errorCode.withException), errCode(errorCode.errorCode())
      , errString(errorCode.errString)
    {}
    operator bool() const
    {
      return errCode == JsNoError;
    }
    JsErrorCode errorCode() const{return errCode;}
    QString errorString() const{return errString;}
    ErrorCode& fatal(JsErrorCode errorCode, const QString& errorMessage = "") noexcept (false)
    {return (*this)(errorCode, errorMessage, FATAL);}
    ErrorCode& warning(JsErrorCode errorCode, const QString& errorMessage = "") noexcept (false)
    {return (*this)(errorCode, errorMessage, WARNING);}
    ErrorCode& info(JsErrorCode errorCode, const QString& errorMessage = "") noexcept (false)
    {return (*this)(errorCode, errorMessage, INFO);}
    ErrorCode& critical(JsErrorCode errorCode, const QString& errorMessage = "") noexcept (false)
    {return (*this)(errorCode, errorMessage, CRITICAL);}
    ErrorCode& debug(JsErrorCode errorCode, const QString& errorMessage = "") noexcept (false)
    {return (*this)(errorCode, errorMessage, DEBUG);}
    ErrorCode& operator()(JsErrorCode errorCode, const QString& errorMessage, const ErrorLevel& errorLevel = WARNING) noexcept (false)
    {
      this->errCode = errorCode;
      if (errorCode == JsNoError)
        return *this;
      errString = "[%1:%2] Error on Error! Original error code=%3, new error code=%4; extra info=%5";
      char buffer [EXCEPTION_MAX_LENGTH + 1];
      size_t actualLength;
      JsErrorCode error;

      JsValueRef exception;
      if ((error = JsGetAndClearException(&exception)) != JsNoError)
        qFatal("%s", errString.arg(__FILE__).arg(__LINE__).arg(errorCode).arg(error).arg(errorMessage).toUtf8().data());

      JsPropertyIdRef messageName;
      if ((error = JsCreatePropertyId("message", strlen("message"), &messageName)) != JsNoError)
        qFatal("%s", errString.arg(__FILE__).arg(__LINE__).arg(errorCode).arg(error).arg(errorMessage).toUtf8().data());

      JsValueRef messageValue;
      if ((error = JsGetProperty(exception, messageName, &messageValue)) != JsNoError)
        qFatal("%s", errString.arg(__FILE__).arg(__LINE__).arg(errorCode).arg(error).arg(errorMessage).toUtf8().data());
      if ((error = JsCopyString(messageValue, buffer, EXCEPTION_MAX_LENGTH, &actualLength)) != JsNoError)
        qFatal("%s", errString.arg(__FILE__).arg(__LINE__).arg(errorCode).arg(error).arg(errorMessage).toUtf8().data());

      buffer[actualLength] = 0;
      errString = QString("[%1:%2] %3; extra info=%5").arg(__FILE__).arg(__LINE__).arg(buffer).arg(errorMessage);
      switch(errorLevel)
      {
        case DEBUG: qDebug("%s", errString.toUtf8().data());break;
        case INFO: qInfo("%s", errString.toUtf8().data());break;
        case WARNING: qWarning("%s", errString.toUtf8().data());break;
        case CRITICAL: qCritical("%s", errString.toUtf8().data());break;
        case FATAL: qFatal("%s", errString.toUtf8().data());
      }
      if (this->withException)
        throw new std::runtime_error(errString.toUtf8().data());
      else
        return *this;

    }
    ErrorCode& operator=(JsErrorCode errorCode) noexcept (false)
    {
      return (*this)(errorCode, "");
    }
    ErrorCode& operator=(const ErrorCode& errorCode) noexcept (true)
    {
      this->errCode = errorCode.errCode;
      this->errString = errorCode.errString;
      this->withException = errorCode.withException;
      return *this;
    }
  };
  struct s_JSEngine_DATA
  {
    JsRuntimeHandle runtime;
    JsContextRef context;
    JsValueRef globalObject;
    ErrorCode errCode;
    QString errorString;
  };
  static QVariant toVariant(JsValueRef value, ErrorCode* errorCode = nullptr);
  static JsValueRef toJsValueRef(const QVariant& value, ErrorCode* errorCode = nullptr);
  static JsErrorCode JsGetPropertyIdFromName(const QString& string, JsPropertyIdRef *propertyId)
  {
    return JsCreatePropertyId(
     string.toUtf8().data(),
     static_cast<size_t>(string.toUtf8().length()),
     propertyId);
  }
  struct s_callback_data
  {
    QObject* object;
    QMetaMethod method;
  };
  #define DATA (*reinterpret_cast<struct s_JSEngine_DATA*>(internalData))

  JSEngine::JSEngine(QObject *parent): QObject(parent), internalData(new s_JSEngine_DATA())
  {
    ErrorCode errCode;
    errCode.critical(JsCreateRuntime(JsRuntimeAttributeNone, nullptr, &DATA.runtime), QString("Error at JsCreateRuntime"));
    errCode.critical(JsCreateContext(DATA.runtime, &DATA.context), "Error at JsCreateContext");
    errCode.critical(JsSetCurrentContext(DATA.context));
    errCode.critical(JsGetGlobalObject(&DATA.globalObject));
    QString handler = QString(R"END(
                              var %1handler = {
                              get: function(target, name) {
                                 if (name in target)
                                    return target[name];
                                 return target.%1get(name);
                              },
                              set: function(target, name, value) {
                                 if (name in target)
                                    return;
                                 target.%1set(name, value);
                              }
                              };
                      )END").arg(JS_EXPORT_BASE_NAME);
    evaluate(handler);
  }
  JSEngine::~JSEngine()
  {
    JsSetCurrentContext(JS_INVALID_REFERENCE);
    JsDisposeRuntime(DATA.runtime);
    delete reinterpret_cast<struct s_JSEngine_DATA*>(internalData);
  }

  QVariant JSEngine::evaluate(const QString &program, const QString &fileName, int)
  {
    static unsigned currentSourceContext = 0;
    JsValueRef result;
    DATA.errCode = ErrorCode(JsNoError, false);
    ErrorCode& errCode = DATA.errCode;
    JsValueRef fname;
    if (!(errCode = JsCreateString(fileName.toUtf8().data(), static_cast<size_t>(fileName.toUtf8().size()), &fname)))
      return QVariant();


    JsValueRef scriptSource;
    if (!(errCode = JsCreateString(program.toUtf8().data(), static_cast<size_t>(program.toUtf8().size()), &scriptSource)))
      return QVariant();

    if (!(errCode = JsRun(scriptSource, currentSourceContext++, fname, JsParseScriptAttributeNone, &result)))
      return QVariant();

    return toVariant(result);
  }
  bool JSEngine::errorEncountered() const
  {
    return ! DATA.errCode;
  }
  bool JSEngine::collectGarbage()
  {
    ErrorCode errCode(JsCollectGarbage(DATA.runtime), false);
    return errCode;
  }
  QString JSEngine::errorString() const
  {
    return DATA.errCode.errorString();
  }
  bool JSEngine::registerValue(const QString& name, const QVariant& value)
  {
    JsValueRef jsValue;
    JsPropertyIdRef propertyId;
    DATA.errCode = ErrorCode(JsNoError, false);
    ErrorCode& errCode = DATA.errCode;
    if (!(errCode = JsCreatePropertyId(name.toUtf8().data(), static_cast<size_t>(name.toUtf8().size()), &propertyId)))
      return false;
    jsValue = toJsValueRef(value, &errCode);
    if (!errCode) return false;
    if (!(errCode = JsSetProperty(DATA.globalObject, propertyId, jsValue, true)))
      return false;
    return true;
  }
  bool JSEngine::registerValue(QObject* value, const QString& name)
  {
    if (value == nullptr)
      qFatal("[%s:%d] Null pointer given\n", __FILE__, __LINE__);
    DATA.errCode = ErrorCode(JsNoError, false);
    ErrorCode& errCode = DATA.errCode;
    QString objectName = name;
    if (objectName.isEmpty())
    {
      if (value->objectName().isEmpty())
        qFatal("[%s:%d] Asked to register an object without a name given\n", __FILE__, __LINE__);
      objectName = value->objectName();
    }
    JsValueRef object;
    JsPropertyIdRef objectPropertyId;

    if(!(errCode = JsCreateObject(&object))) return false;
    if (!(errCode = JsGetPropertyIdFromName((JS_EXPORT_BASE_NAME+objectName).toUtf8().data(), &objectPropertyId))) return false;
    if (!(errCode = JsSetProperty(DATA.globalObject, objectPropertyId, object, true))) return false;
    const QMetaObject* metaObject = value->metaObject();
    QMetaMethod method;
    for(int i = 0, size = metaObject->methodCount(); i < size ; i++)
    {
      method = metaObject->method(i);
      if (method.access() != QMetaMethod::Public)
        continue;
      s_callback_data* callbackData = new s_callback_data{value, method};
      if (registerFunction(object, method.name(), caller, callbackData) == false)
        return false;
    }
    s_callback_data* callbackData = new s_callback_data{value, method};
    if (registerFunction(object, JS_EXPORT_BASE_NAME "get", get, callbackData) == false) return false;
    if (registerFunction(object, JS_EXPORT_BASE_NAME "set", set, callbackData) == false) return false;
    evaluate(QString("var %1 = new Proxy(%2%1, %2handler);4").arg(objectName).arg(JS_EXPORT_BASE_NAME));
    return errCode;
  }
  QVariant JSEngine::value(const QString& name, bool *ok)
  {
    DATA.errCode = ErrorCode(JsNoError, false);
    ErrorCode& errCode = DATA.errCode;
    JsPropertyIdRef propertyId;
    JsValueRef symbol;
    QVariant retour;
    if (ok != nullptr) *ok = false;
    if (!(errCode = JsGetPropertyIdFromName(name, &propertyId)))
      return QVariant();
    if (!(errCode = JsGetProperty(DATA.globalObject,propertyId, &symbol)))
      return QVariant();
    retour = toVariant(symbol, &errCode);
    if (!errCode)
      return QVariant();
    if (ok != nullptr) *ok = true;

    return retour;
  }
  JsValueRef toJsValueRef(const QVariant& value, ErrorCode* errorCode)
  {
    int i = 0, size = 0;
    JsValueRef retour = JS_INVALID_REFERENCE;
    JsValueRef index  = JS_INVALID_REFERENCE;
    JsValueRef subValue;
    ErrorCode errCode(JsNoError, false);
    QVariantList array;
    JsPropertyIdRef propertyIdForName;
    QVariantMap object;
    if (value.isNull())
     errCode = JsGetNullValue(&retour);
    else
      switch(value.type())
      {
        case QVariant::Int:
          errCode = JsIntToNumber(value.toInt(), &retour);
          break;
        case QVariant::Double:
        case QVariant::UInt:
        case QVariant::ULongLong:
          errCode = JsDoubleToNumber(value.toDouble(), &retour);
          break;
        case QVariant::Bool:
          errCode = JsBoolToBoolean(value.toBool(), &retour);
          break;
        case QVariant::String:
          errCode = JsCreateString(value.toString().toUtf8().data(), static_cast<size_t>(value.toString().toUtf8().length()), &retour);
          break;
        case QVariant::Invalid:
          errCode = JsGetUndefinedValue(&retour);
          break;
        case QVariant::Map:
          object = value.toMap();
          if (!(errCode = JsCreateObject(&retour)))
            break;
        {
          QMapIterator<QString, QVariant> it(object);
          while (it.hasNext())
          {
              it.next();
              subValue = toJsValueRef(it.value(), &errCode);
              if (!errCode) break;
              if (!(errCode = JsGetPropertyIdFromName(it.key(), &propertyIdForName))) break;
              if (!(errCode = JsSetProperty(retour, propertyIdForName, subValue, true))) break;;
          }
        }
          break;
        case QVariant::StringList:
        case QVariant::List:
          array = value.toList();
          if (!(errCode = JsCreateArray(static_cast<unsigned>(array.count()), &retour))) break;
          for(i = 0, size = array.size() ; i < size ; i++)
          {
            if (!(errCode = JsIntToNumber(i,&index))) break;
            subValue = toJsValueRef(array.at(i), &errCode);
            if (!errCode) break;
            errCode = JsSetIndexedProperty(retour, index, subValue);
          }
          break;
        default:
          qWarning("%s", QString("[WARNING] [%1:%2] Unsupported type; value=>%3<, typeName=%4")
                   .arg(__FILE__)
                   .arg(__LINE__)
                   .arg(value.toString())
                   .arg(value.typeName()).toUtf8().data());
          retour = JS_INVALID_REFERENCE;
          break;
      }
    if (errorCode != nullptr) *errorCode = errCode.errorCode();
    if (errCode.errorCode() == JsNoError)
      return retour;
    return JS_INVALID_REFERENCE;
  }
  struct s_node
  {
    bool isScalar;
    bool isArray;
    QVariantList* array;
    QVariantMap* object;
    QVariant* scalar;
    JsValueRef* ref;
  };
  QVariant convert(JsValueRef ref, ErrorCode& errorCode)
  {
    bool boolValue;
    double doubleValue;
    size_t actualLength = 0;
    int i = 0;
    int size = 0;
    JsValueRef tmp;
    QByteArray stringValue;
    JsValueType valueType;
    JsValueRef cell;
    JsValueType cellType;
    JsPropertyIdRef propertyIdForLength;
    QVariantList array;

    QVariantMap object;
    QString     objectKey;
    JsPropertyIdRef propertyIdForName;
    errorCode = JsGetValueType(ref, &valueType);
    if (!errorCode) return QVariant();

    switch(valueType)
    {
      case JsFunction:    return QString("[FUNCTION]");
      case JsError:       return QString("[ERROR]");
      case JsSymbol:      return QString("[SYMBOL]");
      case JsDataView:    return QString("[DATAVIEW]");
      case JsArrayBuffer: return QString("[ARRAYBUFFER]");
      case JsTypedArray:  return QString("[TYPEDARRAY]");
      case JsUndefined:   return QVariant(QVariant::Invalid);
      case JsNull:        return QVariant();
      case JsBoolean: errorCode = JsBooleanToBool(ref, &boolValue);return boolValue;
      case JsNumber:  errorCode = JsNumberToDouble(ref, &doubleValue);return doubleValue;
      case JsString:
        errorCode = JsCopyString(ref, nullptr, 0, &actualLength);
        stringValue.resize(static_cast<int>(actualLength) + 1);
        errorCode = JsCopyString(ref, stringValue.data(), actualLength, &actualLength);
        stringValue[static_cast<int>(actualLength)] = 0;
        return QString(stringValue);
      case JsObject:
        JsValueRef propertyNamesArray;
        if (!(errorCode = JsGetOwnPropertyNames(ref, &propertyNamesArray))) return QVariant();
        if (!(errorCode = JsGetPropertyIdFromName("length", &propertyIdForLength))) return QVariant();
        if (!(errorCode = JsGetProperty(propertyNamesArray, propertyIdForLength, &tmp))) return QVariant();
        if (!(errorCode = JsNumberToInt(tmp, &size))) return QVariant();
        for(i = 0 ; i < size ; i++)
        {
          if (!(errorCode = JsIntToNumber(i,&tmp))) return QVariant();
          if (!(errorCode = JsGetIndexedProperty(propertyNamesArray, tmp, &cell))) return QVariant();
          objectKey = convert(cell, errorCode).toString();
          if (!(errorCode = JsGetPropertyIdFromName(objectKey, &propertyIdForName))) return QVariant();
          if (!(errorCode = JsGetProperty(ref, propertyIdForName, &tmp))) return QVariant();
          object.insert(objectKey, convert(tmp, errorCode));
          if (!errorCode) return QVariant();
        }

        return object;
      case JsArray:
        if (!(errorCode = JsGetPropertyIdFromName("length", &propertyIdForLength))) return QVariant();
        if (!(errorCode = JsGetProperty(ref, propertyIdForLength, &tmp))) return QVariant();
        if (!(errorCode = JsNumberToInt(tmp, &size))) return QVariant();
        for(i = 0 ; i < size ; i++)
        {
          if (!(errorCode = JsIntToNumber(i,&tmp))) return QVariant();
          if (!(errorCode = JsGetIndexedProperty(ref, tmp, &cell))) return QVariant();
          if (!(errorCode = JsGetValueType(tmp, &cellType))) return QVariant();
          array.append(convert(cell, errorCode));
          if (!errorCode) return QVariant();
        }
        return array;
    }
    return QVariant();
  }
  QVariant toVariant(JsValueRef value, ErrorCode* errorCode)
  {
    ErrorCode errCode;
    QVariant retour = convert(value, errCode);
    if (errorCode != nullptr) *errorCode = errCode;
    return retour;
  }
}
bool registerFunction(JsValueRef hostObject, const QByteArray &callbackName, JsNativeFunction callback, void *callbackState)
{
    chakracorejsengine::ErrorCode error(false);

    JsPropertyIdRef propertyId;
    if(!(error = JsCreatePropertyId(callbackName.data(),
                     static_cast<size_t>(callbackName.length()),
                     &propertyId))) return false;

    JsValueRef function;
    if(!(error = JsCreateFunction(callback, callbackState, &function))) return false;
    if(!(error = JsSetProperty(hostObject, propertyId, function, true))) return false;
    return true;
}
bool runMethod(QObject* object, QMetaMethod& method, QVariantList& parameters, QVariant& retour)
{
  Q_ASSERT(method.parameterCount() <= parameters.count());
  Q_ASSERT(object != nullptr);
  bool ok;
  QGenericReturnArgument returnArgument;
  QList<QGenericArgument> argumentForCall;
  for(int i = 0 ; i < 10 ; i++)
    argumentForCall.append(QGenericArgument());

  for(int i = 0, size = method.parameterCount() ; i < size ; i++)
  {
    if (method.parameterType(i-1) != parameters.at(i).userType())
    {
      switch(method.parameterType(i))
      {
        case QMetaType::Int:
          parameters[i] = QVariant(parameters.at(i).toInt());
          break;
        case QMetaType::Double:
        case QMetaType::UInt:
        case QMetaType::ULongLong:
          parameters[i] = QVariant(parameters.at(i).toDouble());
          break;
        case QMetaType::Bool:
          parameters[i] = QVariant(parameters.at(i).toBool());
          break;
        case QMetaType::QString:
          parameters[i] = QVariant(parameters.at(i).toString());
          break;
        default:
          qWarning("[WARNING][%s:%d] Unsupported type asked by the method=%s; pos=%d", __FILE__, __LINE__,  method.methodSignature().data(), i);
          return false;
      }
    }
    argumentForCall[i] = QGenericArgument(parameters.at(i).typeName(), parameters.at(i).constData());
  }
  if (method.returnType() == QMetaType::Void || method.returnType() == QMetaType::UnknownType)
  {
    ok = method.invoke(object, Qt::DirectConnection
                          , argumentForCall[0], argumentForCall[1], argumentForCall[2], argumentForCall[3]
                          , argumentForCall[4], argumentForCall[5], argumentForCall[6], argumentForCall[7]
                          , argumentForCall[8], argumentForCall[9]);

  }
  else
  {
    retour = QVariant(QMetaType::type(method.typeName()), nullptr);
    returnArgument = QGenericReturnArgument(method.typeName(), const_cast<void*>(retour.constData()));

    ok = method.invoke(object, Qt::DirectConnection, returnArgument
                             , argumentForCall[0], argumentForCall[1], argumentForCall[2], argumentForCall[3]
                             , argumentForCall[4], argumentForCall[5], argumentForCall[6], argumentForCall[7]
                             , argumentForCall[8], argumentForCall[9]);
  }
  return ok;
}
JsValueRef get(JsValueRef, bool, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
  QVariant returnValue;
  chakracorejsengine::ErrorCode errCode(false);
  if (callbackState == nullptr)
  {
    qCritical("[CRITICAL][%s:%d] Caller called without callBackState\n", __FILE__, __LINE__);
    return JS_INVALID_REFERENCE;
  }
  chakracorejsengine::s_callback_data* data = static_cast<chakracorejsengine::s_callback_data*>(callbackState);
  if (data->object == nullptr)
  {
    qCritical("[CRITICAL][%s:%d] Caller called with an invalid object\n", __FILE__, __LINE__);
    return JS_INVALID_REFERENCE;
  }
  if (argumentCount != 2)
  {
    qWarning("[WARNING][%s:%d] Didn't receive the right parameters number; expected=%d, received=%d\n", __FILE__, __LINE__, data->method.parameterCount(), 2);
    return JS_INVALID_REFERENCE;
  }
  QString name = chakracorejsengine::toVariant(arguments[1], &errCode).toString();
  if (!errCode) return JS_INVALID_REFERENCE;
  const QMetaObject* metaObject = data->object->metaObject();
  int idxProperty = metaObject->indexOfProperty(name.toUtf8().data());
  if (idxProperty < 0)
  {
    qWarning("[WARNING][%s:%d] Asked a property that doesn't exist; objectType=%s, attribut asked=%s\n", __FILE__, __LINE__, metaObject->className(), name.toUtf8().data());
    return JS_INVALID_REFERENCE;
  }

  returnValue = metaObject->property(idxProperty).read(data->object);

  return chakracorejsengine::toJsValueRef(returnValue, &errCode);
}
JsValueRef set(JsValueRef, bool, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
  QVariant returnValue;
  chakracorejsengine::ErrorCode errCode(false);
  if (callbackState == nullptr)
  {
    qCritical("[CRITICAL][%s:%d] Caller called without callBackState\n", __FILE__, __LINE__);
    return JS_INVALID_REFERENCE;
  }
  chakracorejsengine::s_callback_data* data = static_cast<chakracorejsengine::s_callback_data*>(callbackState);
  if (data->object == nullptr)
  {
    qCritical("[CRITICAL][%s:%d] Caller called with an invalid object\n", __FILE__, __LINE__);
    return JS_INVALID_REFERENCE;
  }
  if (argumentCount != 3)
  {
    qWarning("[WARNING][%s:%d] Didn't receive the right parameters number; expected=%d, received=%d\n", __FILE__, __LINE__, data->method.parameterCount(), 3);
    return JS_INVALID_REFERENCE;
  }
  QString  name  = chakracorejsengine::toVariant(arguments[1], &errCode).toString();
  if (!errCode) return JS_INVALID_REFERENCE;
  QVariant value = chakracorejsengine::toVariant(arguments[2], &errCode);
  if (!errCode) return JS_INVALID_REFERENCE;
  const QMetaObject* metaObject = data->object->metaObject();
  int idxProperty = metaObject->indexOfProperty(name.toUtf8().data());
  if (idxProperty < 0)
  {
    qWarning("[WARNING][%s:%d] Asked a property that doesn't exist; objectType=%s, attribut asked=%s\n", __FILE__, __LINE__, metaObject->className(), name.toUtf8().data());
    return JS_INVALID_REFERENCE;
  }

  if (! metaObject->property(idxProperty).write(data->object, value))
    qWarning("[WARNING][%s:%d] Write the propery didn't work; objectType=%s, property=%s, value=%s\n", __FILE__, __LINE__, metaObject->className(), name.toUtf8().data(), value.toString().toUtf8().data());


  return JS_INVALID_REFERENCE;
}
JsValueRef caller(JsValueRef, bool, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
  bool ok;
  QVariant returnValue;
  QGenericReturnArgument returnArgument;
  chakracorejsengine::ErrorCode errCode(false);
  if (callbackState == nullptr)
  {
    qCritical("[CRITICAL][%s:%d] Caller called without callBackState\n", __FILE__, __LINE__);
    return JS_INVALID_REFERENCE;
  }
  chakracorejsengine::s_callback_data* data = static_cast<chakracorejsengine::s_callback_data*>(callbackState);
  if (data->object == nullptr)
  {
    qCritical("[CRITICAL][%s:%d] Caller called with an invalid object\n", __FILE__, __LINE__);
    return JS_INVALID_REFERENCE;
  }
  if (data->method.parameterCount() != argumentCount-1)
  {
    qWarning("[WARNING][%s:%d] Didn't receive the right parameters number; expected=%d, received=%d\n", __FILE__, __LINE__, data->method.parameterCount(), argumentCount-1);
    return JS_INVALID_REFERENCE;
  }
  if (argumentCount > 11)
  {
    qWarning("[WARNING][%s:%d] Received too much parameters;  max expected=10, received=%d\n", __FILE__, __LINE__, argumentCount-1);
    return JS_INVALID_REFERENCE;
  }
  QList<QGenericArgument> argumentForCall;
  for(int i = 0 ; i < 10 ; i++)
    argumentForCall.append(QGenericArgument());
  QVariantList variantArguments;
  for(int i = 1, size = argumentCount ; i < size ; i++)
  {
    variantArguments.append(chakracorejsengine::toVariant(arguments[i], &errCode));
    if (!errCode)
      return JS_INVALID_REFERENCE;
  }
  ok = runMethod(data->object, data->method, variantArguments, returnValue);
  if (!ok)
  {
    qWarning("[WARNING][%s:%d] Call of >%s< failed", __FILE__, __LINE__, data->method.methodSignature().data());
    return JS_INVALID_REFERENCE;
  }
  if (!returnValue.isValid())
    return JS_INVALID_REFERENCE;
  return chakracorejsengine::toJsValueRef(returnValue, &errCode);
}
