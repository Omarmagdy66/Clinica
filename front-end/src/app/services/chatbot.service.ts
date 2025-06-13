import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export interface ChatRequest {
  question: string;
}

export interface ChatResponse {
  answer: string;
}

@Injectable({
  providedIn: 'root'
})
export class ChatbotService {
  private apiUrl = `${environment.apiUrl}/api/Chatbot`;

  constructor(private http: HttpClient) { }

  /**
   * إرسال سؤال إلى Chatbot والحصول على إجابة
   * @param question السؤال المراد إرساله
   * @returns Observable يحتوي على إجابة Chatbot
   */
  sendMessage(question: string): Observable<any> {
    const request: ChatRequest = { question };
    console.log('Sending request to:', this.apiUrl);
    console.log('Request payload:', request);
    
    return this.http.post(`${this.apiUrl}/send`, request).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('API Error:', error);
    let errorMessage = 'حدث خطأ في الاتصال';
    
    if (error.error instanceof ErrorEvent) {
      // خطأ في جانب العميل
      errorMessage = `خطأ: ${error.error.message}`;
    } else {
      // خطأ في جانب الخادم
      errorMessage = `رمز الخطأ: ${error.status}\nالرسالة: ${error.message}`;
    }
    
    return throwError(() => errorMessage);
  }
} 