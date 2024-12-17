// src/app/components/searchable-cards/searchable-cards.component.ts

import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Verse } from '../models/verse.model';
import { VerseService } from '../services/verse.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-searchable-cards',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './searchable-cards.component.html',
  styleUrls: ['./searchable-cards.component.scss']
})
export class SearchableCardsComponent implements OnInit, OnDestroy {
  searchTerm: string = '';
  filteredVerses: Verse[] = [];
  isLoading: boolean = false;
  errorMessage: string = '';
  searchInitiated: boolean = false;
  lastValidSearchTerm: string = '';

  recentSearches: string[] = [];
  private readonly MAX_RECENT_SEARCHES = 5;

  private searchSubscription: Subscription | undefined;

  constructor(private verseService: VerseService, private router: Router) { }

  ngOnInit(): void {
    this.loadRecentSearches();
  }

  ngOnDestroy(): void {
    this.searchSubscription?.unsubscribe();
  }

  /**
   * Load recent searches from localStorage
   */
  private loadRecentSearches(): void {
    const searches = localStorage.getItem('recentSearches');
    if (searches) {
      this.recentSearches = JSON.parse(searches);
    }
  }

  /**
   * Save recent searches to localStorage
   */
  private saveRecentSearches(): void {
    localStorage.setItem('recentSearches', JSON.stringify(this.recentSearches));
  }

  /**
   * Handle form submission to perform search
   * @param event - The form submission event
   */
  onSearchSubmit(event: Event): void {
    event.preventDefault();
    const trimmedTerm = this.searchTerm.trim();

    if (trimmedTerm.length < 5) {
      this.errorMessage = 'Please enter at least 5 characters to search.';
      return;
    }

    this.errorMessage = '';
    this.performSearch(trimmedTerm);
  }

  /**
   * Perform the search by fetching verses from the backend based on the search term
   * @param term - The search term
   */
  private performSearch(term: string): void {
    this.isLoading = true;
    this.searchInitiated = true;
    this.lastValidSearchTerm = term;

    this.addToRecentSearches(term);

    this.searchSubscription = this.verseService.getVerses(term).subscribe({
      next: (verses: Verse[]) => {
        this.filteredVerses = verses;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching verses:', err);
        this.errorMessage = 'Failed to load verses. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  /**
   * Add a search term to the recent searches list
   * @param term - The search term to add
   */
  private addToRecentSearches(term: string): void {
    this.recentSearches = this.recentSearches.filter(t => t.toLowerCase() !== term.toLowerCase());
    this.recentSearches.unshift(term);
    if (this.recentSearches.length > this.MAX_RECENT_SEARCHES) {
      this.recentSearches.pop();
    }
    this.saveRecentSearches();
  }

  /**
   * Handle clicking on a recent search term
   * @param term - The search term to perform
   */
  onRecentSearchClick(term: string): void {
    this.searchTerm = term;
    this.errorMessage = '';
    this.performSearch(term);
  }

  /**
   * Clear all recent searches
   */
  clearRecentSearches(): void {
    this.recentSearches = [];
    this.saveRecentSearches();
  }
}
