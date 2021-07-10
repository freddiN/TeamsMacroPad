
int pin_mute  = 2;
int pin_cam   = 4;
int pin_share = 6;

void setup() {
  pinMode(pin_mute, INPUT_PULLUP);
  pinMode(pin_cam, INPUT_PULLUP);
  pinMode(pin_share, INPUT_PULLUP);
  
  Serial.begin(115200);
  while(!Serial) { 
  }
}

void loop() {
    if (digitalRead(pin_mute) == LOW) {
      Serial.println("TOGGLE_BUTTON_1");
      delay(500);
    } else if (digitalRead(pin_cam) == LOW) {
      Serial.println("TOGGLE_BUTTON_2");
      delay(500);
    } else if (digitalRead(pin_share) == LOW) {
      Serial.println("TOGGLE_BUTTON_3");
      delay(500);
    }
}
