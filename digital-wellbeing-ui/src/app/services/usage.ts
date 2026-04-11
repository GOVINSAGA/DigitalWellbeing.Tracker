import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UsageService {

  private http = inject(HttpClient);

  private baseUrl = 'https://localhost:7036/api/usage'; // adjust port

  getSummary() {
    return this.http.get<any[]>(`${this.baseUrl}/summary`);
  }

  getAll() {
    return this.http.get<any[]>(`${this.baseUrl}/all`);
  }
}
