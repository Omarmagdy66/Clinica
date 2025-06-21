<<<<<<< HEAD
import { Component, OnInit } from '@angular/core';
import { ChatbotService } from '../../services/chatbot.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-chatbot',
  templateUrl: './chatbot.component.html',
  styleUrls: ['./chatbot.component.scss']
})
export class ChatbotComponent implements OnInit {
  chatForm: FormGroup;
  messages: { text: string; isUser: boolean }[] = [];
  isLoading = false;

  constructor(
    private chatbotService: ChatbotService,
    private fb: FormBuilder
  ) {
    this.chatForm = this.fb.group({
      message: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    // إضافة رسالة ترحيب
    this.messages.push({
      text: 'مرحباً! كيف يمكنني مساعدتك اليوم؟',
      isUser: false
    });
  }

  sendMessage(): void {
    if (this.chatForm.valid) {
      const message = this.chatForm.get('message')?.value;
      
      // إضافة رسالة المستخدم
      this.messages.push({
        text: message,
        isUser: true
      });

      this.isLoading = true;
      
      // إرسال الرسالة إلى الخدمة
      this.chatbotService.sendMessage(message).subscribe({
        next: (response: any) => {
          console.log('API Response:', response);
          
          // إضافة رد Chatbot
          this.messages.push({
            text: response?.answer || response || 'عذراً، لم أتمكن من فهم سؤالك. هل يمكنك إعادة صياغته؟',
            isUser: false
          });
          this.isLoading = false;
          this.chatForm.reset();
        },
        error: (error: any) => {
          console.error('Error in component:', error);
          this.messages.push({
            text: typeof error === 'string' ? error : 'عذراً، حدث خطأ في الاتصال. يرجى المحاولة مرة أخرى.',
            isUser: false
          });
          this.isLoading = false;
        }
      });
    }
  }
=======
import { Component, OnInit } from '@angular/core';
import { ChatbotService } from '../../services/chatbot.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-chatbot',
  templateUrl: './chatbot.component.html',
  styleUrls: ['./chatbot.component.scss']
})
export class ChatbotComponent implements OnInit {
  chatForm: FormGroup;
  messages: { text: string; isUser: boolean }[] = [];
  isLoading = false;

  constructor(
    private chatbotService: ChatbotService,
    private fb: FormBuilder
  ) {
    this.chatForm = this.fb.group({
      message: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    // إضافة رسالة ترحيب
    this.messages.push({
      text: 'مرحباً! كيف يمكنني مساعدتك اليوم؟',
      isUser: false
    });
  }

  sendMessage(): void {
    if (this.chatForm.valid) {
      const message = this.chatForm.get('message')?.value;
      
      // إضافة رسالة المستخدم
      this.messages.push({
        text: message,
        isUser: true
      });

      this.isLoading = true;
      
      // إرسال الرسالة إلى الخدمة
      this.chatbotService.sendMessage(message).subscribe({
        next: (response: any) => {
          console.log('API Response:', response);
          
          // إضافة رد Chatbot
          this.messages.push({
            text: response?.answer || response || 'عذراً، لم أتمكن من فهم سؤالك. هل يمكنك إعادة صياغته؟',
            isUser: false
          });
          this.isLoading = false;
          this.chatForm.reset();
        },
        error: (error: any) => {
          console.error('Error in component:', error);
          this.messages.push({
            text: typeof error === 'string' ? error : 'عذراً، حدث خطأ في الاتصال. يرجى المحاولة مرة أخرى.',
            isUser: false
          });
          this.isLoading = false;
        }
      });
    }
  }
>>>>>>> 8353e6b8191821f7a21f7a21f1f2e1a60dab3cc6
} 