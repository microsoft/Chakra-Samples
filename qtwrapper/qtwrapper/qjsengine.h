#ifndef QJSENGINE_H
#define QJSENGINE_H

#include <QtCore/qobject.h>


namespace chakracorejsengine
{
/**
 * @brief The JSEngine class
 *
 * You must create ONE JSEngine per thread at most!
 */
class JSEngine : public QObject
{
    Q_OBJECT
public:
    explicit JSEngine(QObject *parent = nullptr);
    virtual ~JSEngine();

    QVariant evaluate(const QString &program, const QString &fileName = QString(), int lineNumber = 1);
    bool registerValue(const QString& name, const QVariant& value);
    bool registerValue(QObject* value, const QString& name = QString());
    QVariant value(const QString& name, bool *ok = nullptr);
    QString errorString() const;
    bool errorEncountered() const;
    bool collectGarbage();

private:
    Q_DISABLE_COPY(JSEngine)
    void* internalData;
};
}

#endif // QJSENGINE_H
