import { AfterViewInit, Component } from '@angular/core';
import { SearchableCardsComponent } from './searchable-cards/searchable-cards.component';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [SearchableCardsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements AfterViewInit {
  title = 'BibleSemanticSearcher.Web';

  ngAfterViewInit(): void {
    const spinner = document.getElementById("globalSpinner");
    if (spinner) {
      spinner.style.display = "none";
    }
  }
}
