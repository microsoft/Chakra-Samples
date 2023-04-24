#ifndef QTOBJECT_H
#define QTOBJECT_H

#include <QObject>

class QtObject : public QObject
{
  Q_OBJECT
  QString name;
public:
  Q_PROPERTY(QString name READ getName WRITE setName)
  explicit QtObject(QObject *parent = nullptr);
  QString getName()const{return name;}
  void setName(const QString& name){this->name = name;}
signals:

public slots:
  void echo(const QString& message);
};

#endif // QTOBJECT_H
