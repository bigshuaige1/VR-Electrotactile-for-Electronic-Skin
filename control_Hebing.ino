#define DATA 17 // 595data A3
#define STCP 18 // 595stcp A4
#define SHCP 19 // 595shcp A5

String inputalpha = "";
String indate = "12345678123456781234567812345678";
String indate1 = "a2345678a23456781234567812345678";
String indate2 = "1a3456781a3456781234567812345678";
String indate3 = "12a4567812a456781234567812345678";
String indate4 = "123a5678123a56781234567812345678";
String indate5 = "1234a6781234a6781234567812345678";
String indate6 = "12345a7812345a781234567812345678";
String indate7 = "123456a8123456a81234567812345678";
String indate8 = "1234567a1234567a1234567812345678";
String indate9 = "1234567812345678a2345678a2345678";
String indate10 = "12345678123456781a3456781a345678";
String indate11 = "123456781234567812a4567812a45678";
String indate12 = "1234567812345678123a5678123a5678";
String indate18 = "aaaaaaaaaaaaaaaaaaaaaaaaaaaa5678";
String* encoder[] = {
  &indate1, &indate2, &indate3, 
  &indate4, &indate5, &indate6, 
  &indate7, &indate8, &indate9, 
  &indate10, &indate11, &indate12
};

String indate0String = "";     
String val = "";
int i = 0;
int state = 0;
// 中断引脚定义
#define Voltage_Input 3 // D3

void Send595(char input) {
  // input变量有4个情况，'A','B','C','N'。分别对应ABC通道和悬空情况。
  // A、B、C分别为微电流的正、地、负
  switch (input) {
    case 'a':
    case 'A':
      digitalWrite(DATA, 1); // A通道连上
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 0); // B通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 0); // C通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      break;

    case 'b':
    case 'B':
      digitalWrite(DATA, 0); // A通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 1); // B通道连上
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 0); // C通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      break;

    case 'c':
    case 'C':
      digitalWrite(DATA, 0); // A通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 0); // B通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 1); // C通道连上
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      break;
      
    case 'd':
    case 'D':
      digitalWrite(DATA, 1); // A通道连上
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 0); // B通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 1); // C通道连上
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      break;

    case 'n':
    case 'N':
    default:
      digitalWrite(DATA, 0); // A通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 0); // B通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      digitalWrite(DATA, 0); // C通道断开
      digitalWrite(SHCP, HIGH); //_nop_();
      digitalWrite(SHCP, LOW); //_nop_();
      break;
  }
}

void setState(String& onIndicate, String& offIndicate, int onTime, int offTime) {
  char buf_on[onIndicate.length() + 1];
  char buf_off[offIndicate.length() + 1];
  onIndicate.toCharArray(buf_on, onIndicate.length() + 1);
  offIndicate.toCharArray(buf_off, offIndicate.length() + 1);

  for (int i = 0; i < onIndicate.length(); i++) {
    Send595(buf_on[i]);
  }
  digitalWrite(STCP, LOW); //_nop_();
  digitalWrite(STCP, HIGH); //_nop_();
  delay(onTime);

  for (int i = 0; i < offIndicate.length(); i++) {
    Send595(buf_off[i]);
  }
  digitalWrite(STCP, LOW); //_nop_();
  digitalWrite(STCP, HIGH); //_nop_();
  delay(offTime);
}

void setup() {
  Serial.begin(9600);
  Serial.println("COM is ready");
  Serial.println("Input Command :");

  pinMode(DATA, OUTPUT);
  pinMode(SHCP, OUTPUT);
  pinMode(STCP, OUTPUT);

  digitalWrite(DATA, LOW);
  digitalWrite(SHCP, LOW);
  digitalWrite(STCP, LOW);
}
void loop() 
{
  if (Serial.available() > 0) {
    int state = Serial.parseInt(); // 直接解析整数
    
    if (state == 18) { // 震动开启
      Serial.println("震动开启");
      char buf1[indate18.length() + 1];
      indate18.toCharArray(buf1, sizeof(buf1));
      for (char c : buf1) {
        if (c == '\0') break;
        Send595(c);
      }
      digitalWrite(STCP, HIGH);
      delay(1);
    } 
    else if (state == 19) { // 震动关闭
      Serial.println("震动关闭");
      char buf2[indate.length() + 1];
      indate.toCharArray(buf2, sizeof(buf2));
      for (char c : buf2) {
        if (c == '\0') break;
        Send595(c);
      }
      digitalWrite(STCP, HIGH);
    }
  }
  
  // //读取电脑传来的数据
  // while(Serial.available()>0)
  // {
  //   indate0String+=char(Serial.read());
  //   Serial.println(indate0String);
  //   state=indate0String.toInt();
  //   delay(2);
  // }
  // if(indate0String.length()>0)
  // {
  //    //Serial.println(indate)
  //    //将indate暂存到val中
  //   if (state==1)
  //   {
  //     Serial.println("state=1");
  //     setState(*encoder[5], indate, 100, 100);
  //     setState(*encoder[4], indate, 100, 100);
  //     setState(*encoder[3], indate, 100, 100);
  //     }  
  //   else if (state==2)
  //   {
  //     Serial.println("state=2");
  //     setState(*encoder[10], indate, 100, 100);
  //     setState(*encoder[7], indate, 100, 100);
  //     setState(*encoder[4], indate, 100, 100);
  //     setState(*encoder[1], indate, 100, 100);    
  //     } 
  //   else if (state==3)
  //   {
  //     Serial.println("state=3");
  //     setState(*encoder[6], indate, 100, 100);
  //     setState(*encoder[4], indate, 100, 100);
  //     setState(*encoder[2], indate, 100, 100);
  //     } 
  //   else if (state==4)
  //   {
  //     Serial.println("state=4");
  //     setState(*encoder[8], indate, 100, 100);
  //     setState(*encoder[4], indate, 100, 100);
  //     setState(*encoder[0], indate, 100, 100);
  //     }   
  //   else if (state==5)
  //   {
  //     Serial.println("state=5");
  //     setState(*encoder[5], indate, 100, 1);
  //     setState(*encoder[4], indate, 100, 1);
  //     setState(*encoder[3], indate, 100, 1);
  //     setState(*encoder[7], indate, 100, 1);
  //     setState(*encoder[4], indate, 100, 1);
  //     setState(*encoder[1], indate, 100, 1);
  //     }
  //   else if (state==6)
  //   {
  //     Serial.println("state=6");
  //     setState(*encoder[6], indate, 100, 1);
  //     setState(*encoder[4], indate, 100, 1);
  //     setState(*encoder[2], indate, 100, 1);
  //     setState(*encoder[8], indate, 100, 1);
  //     setState(*encoder[4], indate, 100, 1);
  //     setState(*encoder[0], indate, 100, 1);
  //     }
  //    else if (state==7)
  //    {
  //     Serial.println("state=7");
  //     setState(*encoder[8], indate, 100, 1);
  //     setState(*encoder[7], indate, 100, 1);
  //     setState(*encoder[6], indate, 100, 1);
  //     setState(*encoder[3], indate, 100, 1);
  //     setState(*encoder[0], indate, 100, 1);
  //     setState(*encoder[1], indate, 100, 1);
  //     setState(*encoder[2], indate, 100, 1);
  //     setState(*encoder[5], indate, 100, 1);
  //     setState(*encoder[8], indate, 100, 1);
  //     } 
  //    else if (state==8)
  //    {
  //      Serial.println("state=8");
  //      setState(*encoder[11], indate, 100, 1);
  //       setState(*encoder[10], indate, 100, 1);
  //       setState(*encoder[7], indate, 100, 1);
  //       setState(*encoder[4], indate, 100, 1);  
  //       setState(*encoder[1], indate, 100, 1);
  //       setState(*encoder[2], indate, 100, 1);
  //       setState(*encoder[5], indate, 100, 1);
  //       setState(*encoder[8], indate, 100, 1);
  //       setState(*encoder[11], indate, 100, 1);
  //     }
  //     else if (state==9)
  //    {
  //      Serial.println("state=9");
  //      setState(*encoder[11], indate, 100, 1);
  //       setState(*encoder[9], indate, 100, 1);
  //       setState(*encoder[8], indate, 100, 1);
  //       setState(*encoder[4], indate, 100, 1);
  //       setState(*encoder[1], indate, 100, 1);
  //       setState(*encoder[4], indate, 100, 1);
  //       setState(*encoder[6], indate, 100, 1);
  //     }  
  //     else if (state==10)
  //    {
  //      Serial.println("state=10");
  //      setState(*encoder[11], indate, 100, 1);
  //       setState(*encoder[9], indate, 100, 1);
  //       setState(*encoder[2], indate, 100, 1);
  //       setState(*encoder[4], indate, 100, 1);
  //       setState(*encoder[7], indate, 100, 1);
  //       setState(*encoder[4], indate, 100, 1);
  //       setState(*encoder[0], indate, 100, 1);
  //     }
      
  //     //
  //     else if (state==11)
  //    {
  //      Serial.println("state=白板");
  //       setState(*encoder[0], indate, 100, 1);
  //       setState(*encoder[1], indate, 100, 1);
  //       setState(*encoder[2], indate, 100, 1);
  //       setState(*encoder[5], indate, 100, 1);
  //       setState(*encoder[8], indate, 100, 1);
  //       setState(*encoder[11], indate, 100, 1);
  //       setState(*encoder[10], indate, 100, 1);
  //       setState(*encoder[9], indate, 100, 1);
  //       setState(*encoder[6], indate, 100, 1);
  //       setState(*encoder[3], indate, 100, 1);
  //     }
  //     else if (state==12)
  //    {
  //      Serial.println("state=二条");
  //      setState(*encoder[10], indate, 100, 1);
  //      setState(*encoder[7], indate, 100, 400);
  //      setState(*encoder[4], indate, 100, 1);
  //      setState(*encoder[1], indate, 100, 1);
  //     }
  //     else if (state==13)
  //    {
  //      Serial.println("state=三条");
  //       setState(*encoder[10], indate, 100, 1);
  //       setState(*encoder[7], indate, 100, 400);
  //       setState(*encoder[5], indate, 100, 1);
  //       setState(*encoder[2], indate, 100, 400);
  //       setState(*encoder[3], indate, 100, 1);
  //       setState(*encoder[0], indate, 100, 1);
  //     }
  //     else if (state==14)
  //    {
  //       Serial.println("state=四条");
  //       setState(*encoder[11], indate, 100, 1);
  //       setState(*encoder[8], indate, 100, 400);
  //       setState(*encoder[5], indate, 100, 1);
  //       setState(*encoder[2], indate, 100, 400);
  //       setState(*encoder[9], indate, 100, 1);
  //       setState(*encoder[6], indate, 100, 400);
  //       setState(*encoder[3], indate, 100, 1);
  //       setState(*encoder[0], indate, 100, 1);
  //     }
  //     else if (state==18)
  //    {
  //      Serial.println("state=开");
  //      char buf1[indate18.length()+1];
  //       indate18.toCharArray(buf1,indate18.length()+1);
  //       for (int i = 0; i < indate18.length(); i++) {
  //         Send595(buf1[i]);
  //         }
  //        digitalWrite(STCP, LOW); //_nop_();
  //        digitalWrite(STCP, HIGH); //_nop_();
  //        delay(1);     
  //     }
  //     else if (state==19)
  //    {
  //      Serial.println("state=关");
  //      char buf2[indate.length()+1];
  //       indate.toCharArray(buf2,indate.length()+1);
  //       for (int i = 0; i < indate.length(); i++) {
  //         Send595(buf2[i]);
  //         }
  //        digitalWrite(STCP, LOW); //_nop_();
  //        digitalWrite(STCP, HIGH); //_nop_();
  //     }
                          
  //   //Serial.println(indate);

  // }   
  //   indate0String="";   //清空indate为下一次输入做准备

}
