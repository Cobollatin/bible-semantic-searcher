import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Verse } from '../models/verse.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VerseService {
  constructor(private http: HttpClient) { }

  /**
   * Fetch verses based on the search term.
   * If useMockData is true, perform client-side filtering.
   * Otherwise, send the search term as a query parameter to the backend API.
   * @param searchTerm - The term to search for (minimum 5 characters)
   * @returns An Observable of Verse array
   */
  getVerses(searchTerm: string = ''): Observable<Verse[]> {
    // Construct the URL with the search query parameter
    const url = `${environment.apiUrl}/api/bible?search=${encodeURIComponent(searchTerm)}`;
    return this.http.get<Verse[]>(url).pipe(
      catchError(this.handleError<Verse[]>('getVerses', []))
    );
  }

  /**
   * Handle HTTP operation that failed.
   * Let the app continue by returning an empty result.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: Error): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}
