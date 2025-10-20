// Curtin University - Mechatronics Engineering
// --------------------------------------------------------------
// Serial Line Following Robot Firmware
// This code handles sensor reading, line detection,
// and DAC-based motor control with GUI serial communication.
// --------------------------------------------------------------

// ========================== CONSTANTS ==========================
const byte START_BYTE = 255;            // Start byte for serial messages
const byte NUM_DAC_BITS = 8;            // Number of bits for DAC output
const int LINE_THRESHOLD = 400;         // Threshold for black/white detection
const unsigned long RECOVERY_TIMEOUT = 2000; // Timeout (ms) before stopping on line loss
const byte MOTOR_SLOW_SPEED = 130;       // Motor speed during recovery turns
const byte MOTOR_REVERSE_SPEED = 75;   // Motor speed when backing up
const byte MOTOR_IDLE_SPEED = 127;      // Motor neutral/idle speed
const byte LOOP_DELAY_MS = 5;           // Delay between iterations for readability

// Port enumerations
const byte INPUT1 = 0;
const byte INPUT2 = 1;
const byte SPEED_OUTPUT = 2;
const byte ONOFF_CONTROL = 3;
const byte MOTOR1_SPEED = 4;
const byte MOTOR2_SPEED = 5;
const byte TURNING_SPEED = 6;

// Sensor pins
const byte SENSOR1 = A6;
const byte SENSOR2 = A7;

// DAC pins (8-bit per motor)
const byte DACPIN1[NUM_DAC_BITS] = {9, 8, 7, 6, 5, 4, 3, 2};
const byte DACPIN2[NUM_DAC_BITS] = {A2, A3, A4, A5, A1, A0, 11, 10};

// Debug mode flag (set true for serial debug without GUI)
const bool DEBUG_MODE = false;

// ======================= GLOBAL VARIABLES ======================
byte speed = 0;              // Base movement speed
byte turningSpeed = 0;       // Speed difference used when turning
byte motor1Speed = 0;        // DAC output for left motor
byte motor2Speed = 0;        // DAC output for right motor
byte input1 = 0;             // Sensor 1 digital state
byte input2 = 0;             // Sensor 2 digital state
byte onOffState = 0;         // System ON/OFF state (0 = off, 1 = on)

byte startByte = 0;
byte commandByte = 0;
byte dataByte = 0;
byte checkByte = 0;
byte checkSum = 0;

byte lastKnownDirection = 0; // 0 = unknown, 1 = left, 2 = right
unsigned long lostLineTime = 0;

// ========================== FUNCTIONS ==========================

// ---- DAC INITIALIZATION ----
void initDACs() {
  for (int i = 0; i < NUM_DAC_BITS; i++) {
    pinMode(DACPIN1[i], OUTPUT);
    pinMode(DACPIN2[i], OUTPUT);
  }
}

// ---- DAC OUTPUT HANDLING ----
void outputToDAC(byte data, const byte dacPins[NUM_DAC_BITS]) {
  for (int i = 0; i < NUM_DAC_BITS; i++) {
    digitalWrite(dacPins[i], (data >> i) & 1 ? HIGH : LOW);
  }
}

// ---- SERIAL DEBUG PRINT ----
void printReceived(byte start, byte command, byte data, byte check, bool valid) {
  if (!DEBUG_MODE) return;

  Serial.print("RX:[");
  Serial.print(start);
  Serial.print(",");
  Serial.print(command);
  Serial.print(",");
  Serial.print(data);
  Serial.print(",");
  Serial.print(check);
  Serial.print("] ");

  if (valid) {
    Serial.print("OK - ");
    switch (command) {
      case INPUT1: Serial.print("IN1"); break;
      case INPUT2: Serial.print("IN2"); break;
      case SPEED_OUTPUT: Serial.print("SPD"); break;
      case ONOFF_CONTROL: Serial.print("ON/OFF"); break;
      case TURNING_SPEED: Serial.print("TURN"); break;
      default: Serial.print("UNKNOWN"); break;
    }
    Serial.print(" = ");
    Serial.println(data);
  } else {
    Serial.println("BAD_CHECKSUM");
  }
}

// =========================== SETUP =============================
void setup() {
  Serial.begin(9600);
  initDACs();
  pinMode(SENSOR1, INPUT);
  pinMode(SENSOR2, INPUT);

  if (DEBUG_MODE) {
    delay(1000);
    Serial.println("=== DEBUG MODE ENABLED ===");
  }
}

// =========================== MAIN LOOP =========================
void loop() {
  // ---- RECEIVE SERIAL COMMANDS ----
  if (Serial.available() >= 4) {
    startByte = Serial.read();
    if (startByte == START_BYTE) {
      commandByte = Serial.read();
      dataByte = Serial.read();
      checkByte = Serial.read();

      checkSum = startByte + commandByte + dataByte;
      bool checksumValid = (checkByte == checkSum);

      printReceived(startByte, commandByte, dataByte, checkByte, checksumValid);

      if (checksumValid) {
        switch (commandByte) {
          case SPEED_OUTPUT: speed = dataByte; break;
          case TURNING_SPEED: turningSpeed = dataByte; break;
          case ONOFF_CONTROL:
            onOffState = dataByte;
            if (onOffState == 1) { // reset when turned on
              lastKnownDirection = 0;
              lostLineTime = 0;
            }
            break;
          default: break; // INPUT1/INPUT2 handled separately
        }
      }
    }
  }

  // ---- READ SENSOR VALUES ----
  int sensor1Value = analogRead(SENSOR1);
  int sensor2Value = analogRead(SENSOR2);
  input1 = (sensor1Value > LINE_THRESHOLD) ? 1 : 0;
  input2 = (sensor2Value > LINE_THRESHOLD) ? 1 : 0;

  // ---- SEND SENSOR STATES TO GUI ----
  byte checkSum1 = START_BYTE + INPUT1 + input1;
  Serial.write(START_BYTE); Serial.write(INPUT1); Serial.write(input1); Serial.write(checkSum1);

  byte checkSum2 = START_BYTE + INPUT2 + input2;
  Serial.write(START_BYTE); Serial.write(INPUT2); Serial.write(input2); Serial.write(checkSum2);

  // ---- DECISION LOGIC ----
  if (onOffState == 1) {
    if (input1 == 0 && input2 == 0) {
      // Straight path
      motor1Speed = motor2Speed = speed;
      lastKnownDirection = 0;
      lostLineTime = 0;
    }
    else if (input1 == 0 && input2 == 1) {
      // Turn right
      motor1Speed = speed;
      motor2Speed = speed - turningSpeed;
      lastKnownDirection = 2;
      lostLineTime = 0;
    }
    else if (input1 == 1 && input2 == 0) {
      // Turn left
      motor1Speed = speed - turningSpeed;
      motor2Speed = speed;
      lastKnownDirection = 1;
      lostLineTime = 0;
    }
    else if (input1 == 1 && input2 == 1) {
      // Line lost — recovery mode
      if (lostLineTime == 0) lostLineTime = millis();

      if (millis() - lostLineTime > RECOVERY_TIMEOUT) {
        motor1Speed = MOTOR_IDLE_SPEED;
        motor2Speed = MOTOR_IDLE_SPEED;
      } else {
        if (lastKnownDirection == 1) {
          // Recover left
          motor1Speed = MOTOR_SLOW_SPEED;
          motor2Speed = speed;
        } else if (lastKnownDirection == 2) {
          // Recover right
          motor1Speed = speed;
          motor2Speed = MOTOR_SLOW_SPEED;
        } else {
          // Unknown — backup
          motor1Speed = MOTOR_REVERSE_SPEED;
          motor2Speed = MOTOR_REVERSE_SPEED;
        }
      }
    }
  } else {
    // System off — idle mode
    motor1Speed = MOTOR_IDLE_SPEED;
    motor2Speed = MOTOR_IDLE_SPEED;
  }

  // ---- OUTPUT TO MOTORS ----
  outputToDAC(motor1Speed, DACPIN1);
  outputToDAC(motor2Speed, DACPIN2);

  // ---- SEND MOTOR SPEEDS TO GUI ----
  byte checkSum3 = START_BYTE + MOTOR1_SPEED + motor1Speed;
  Serial.write(START_BYTE); Serial.write(MOTOR1_SPEED); Serial.write(motor1Speed); Serial.write(checkSum3);

  byte checkSum4 = START_BYTE + MOTOR2_SPEED + motor2Speed;
  Serial.write(START_BYTE); Serial.write(MOTOR2_SPEED); Serial.write(motor2Speed); Serial.write(checkSum4);

  delay(LOOP_DELAY_MS); // maintain stable communication rate
}