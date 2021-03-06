#include<Wire.h>
const int MPU=0x68;
int AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ;

int estadoB;
int boton = 2;

void setup(){
  pinMode(boton, INPUT);
  
  Wire.begin();
  Wire.beginTransmission(MPU);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);
  Serial.begin(9600);
}
 
void loop(){

  estadoB = digitalRead(boton);
  
  Wire.beginTransmission(MPU);
  Wire.write(0x3B);
  Wire.endTransmission(false);
  Wire.requestFrom(MPU,14,true);
  AcX=Wire.read()<<8|Wire.read();     
  AcY=Wire.read()<<8|Wire.read();
  AcZ=Wire.read()<<8|Wire.read();
  Tmp=Wire.read()<<8|Wire.read();
  GyX=Wire.read()<<8|Wire.read();
  GyY=Wire.read()<<8|Wire.read();
  GyZ=Wire.read()<<8|Wire.read();
   
  Serial.print(GyX); Serial.print(";");
  Serial.print(GyY); Serial.print(";");
  Serial.print(GyZ); Serial.print(";");
  Serial.print(estadoB); Serial.println("");  

  delay(25);
}
