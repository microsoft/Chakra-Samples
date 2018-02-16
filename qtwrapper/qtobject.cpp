#include "qtobject.h"

QtObject::QtObject(QObject *parent) : QObject(parent)
{

}
void QtObject::echo(const QString& message)
{
  qDebug("Internal value name=%s, message=%s", name.toUtf8().data(), message.toUtf8().data());
}
