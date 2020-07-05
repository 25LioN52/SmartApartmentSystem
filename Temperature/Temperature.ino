#include <OneWire.h>
#include <Wire.h>
OneWire oneWire(8);
//Подключаем библиотеку для работы с термометром
#include <DallasTemperature.h>
//Создаем объект sensors, подключенный по OneWire
DallasTemperature sensors(&oneWire);

//Создаем переменные для работы с термометром
DeviceAddress tempDeviceAddress;  //переменная для хранения адреса датчика

float temp1 = 0; //переменная для текущего значения температуры
int setTmp = 24; // переменная для заданного значения температуры

#define RELAY_PIN 12
#define VCC_PIN 4
#define GR_PIN 3
#define FLOOR_PIN 11
#define WATER_RELAY_PIN 10
#define WATER_IN_PIN 9
//Объявим переменную для хранения состояния реле
boolean relayStatus1 = LOW;
boolean floorStatus = LOW;

boolean waterInStatus = HIGH;
boolean waterRelayStatus = HIGH;

//Объявим переменные для задания задержки
long previousMillis1 = 0;
long interval1 = 1000; // интервал опроса датчиков температуры

void setup () {
  Wire.begin(0x10);                // join i2c bus with address #10
  Wire.onReceive(receiveEvent); // register event
  Wire.onRequest(requestEvent);

  Serial.begin(9600);// Запуск СОМ порта.
  Serial.println("Start temperature measurement");
  //Настроим пин для управления реле
  pinMode(RELAY_PIN, OUTPUT);
  pinMode(FLOOR_PIN, OUTPUT);
  pinMode(13, OUTPUT);
  pinMode(VCC_PIN, OUTPUT);
  pinMode(WATER_IN_PIN, INPUT);
  pinMode(WATER_RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, LOW);
  digitalWrite(13, LOW);
  digitalWrite(VCC_PIN, HIGH);
  digitalWrite(FLOOR_PIN, LOW);
  digitalWrite(WATER_RELAY_PIN, HIGH);

  //Инициализируем термодатчик и установим разрешающую способность 12 бит (обычно она установлена по умолчанию, так что последнюю строчку можно опустить)
  sensors.begin();
  //sensors.getAddress(tempDeviceAddress, 0);
}

void loop() {
  //Модуль опроса датчиков и получения сведений о температуре
  //Вызывается 1 раз в секунду
  unsigned long currentMillis1 = millis();
  if (currentMillis1 < previousMillis1) {
    previousMillis1 = currentMillis1;
  }
  if (currentMillis1 - previousMillis1 > interval1) {
    previousMillis1 = currentMillis1;

    //Запуск процедуры измерения температуры
    sensors.setWaitForConversion(false);
    sensors.requestTemperatures();
    sensors.setWaitForConversion(true);

    //delay(750) // задержка для обработки информации внутри термометра, в данном случае можно не задавать

    //Считывание значения температуры
    temp1 = sensors.getTempCByIndex(0);
    Serial.println(temp1);
    Serial.println(setTmp);
    Serial.println(relayStatus1);

    waterInStatus = digitalRead(WATER_IN_PIN);
  }

  //Проверка условия включения/выключения нагревателя
  if ((temp1 + 0.1f) < setTmp && relayStatus1 == LOW) {
    relayStatus1 = HIGH;
    digitalWrite(RELAY_PIN, HIGH);
    digitalWrite(13, HIGH);
  }

  if ((temp1 - 0.1f) > setTmp && relayStatus1 == HIGH) {
    relayStatus1 = LOW;
    digitalWrite(RELAY_PIN, LOW);
    digitalWrite(13, LOW);
  }

  if (!waterInStatus && waterRelayStatus) {
    waterRelayStatus = waterInStatus;
    digitalWrite(WATER_RELAY_PIN, waterRelayStatus);
  }
}

void receiveEvent()
{
  byte type = Wire.read();
  byte result = Wire.read();
  if (type == 0) {
    setTmp = result;
  } else if (type == 1) {
    if (result > 0) {
      floorStatus = HIGH;
    } else {
      floorStatus = LOW;
    }
    digitalWrite(FLOOR_PIN, floorStatus);
  } else if (type == 2) {
    waterRelayStatus = result > 0;
    digitalWrite(WATER_RELAY_PIN, waterRelayStatus);
  }

  Serial.print("Receive ");
  Serial.print(type);
  Serial.print(" ");
  Serial.println(setTmp);
}

void requestEvent()
{
  Serial.print("Request");
  byte sending[4] = {0, 0, 0, 0};
  sending[0] = setTmp;
  sending[1] = ((((byte)temp1) << 1) | relayStatus1) << 1;
  sending[3] = (0 | floorStatus << 1);
  for (int i = 0; i < 4; i++)
  {
    Serial.print(" ");
    Serial.print(sending[i]);
  }

  Wire.write(sending, 4);
}
