#include <QDebug>
#include "qtwrapper/qjsengine.h"
#include "qtobject.h"

int main()
{
  QVariant result;
  QStringList list;
  QMap<QString, QVariant> map;
  QtObject *snouf = new QtObject;

  list << "a" << "b" << "c" << "d" << "1" << "2";
  map["e"] = "f";map["g"] = "h";map["i"] = 3;map["4"] = "5";


  chakracorejsengine::JSEngine jsEngine;
  jsEngine.registerValue("nb", 46);          //register a simple value
  jsEngine.registerValue(snouf, "coucou");   //register a Qt's Object
  jsEngine.registerValue("list", list);      //register a list (as an array)
  jsEngine.registerValue("map", map);        //register a map (as an objet)
  jsEngine.evaluate("coucou.echo('access to all variables: nb='+nb+', list[2]='+list[2]+', map.g='+map.g);");
  if (jsEngine.errorEncountered()) return EXIT_FAILURE;
  result = jsEngine.evaluate("5");           //access to value returned
  qDebug("Value returned: %d", result.toInt());

  return EXIT_SUCCESS;
}
