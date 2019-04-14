import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  public meta: PageMeta;
  public news: NewsItem[];

  private http: HttpClient;
  private baseUrl: string;

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      let source = params.get("source");
      let category = params.get("category");
      let uri = 'api/news';

      // Load headlines from specific endpoints if source or category filter selected.
      if (source) {
        uri += '/source/' + source;
      } else if (category) {
        uri += '/category/' + category;
      }
      this.http.get<NewsItem[]>(this.baseUrl + uri).subscribe(result => {
        this.news = result;

        // Generate second part on the title to show which category or source is selected at the moment.
        if (result.length > 0) {
          if (source) {
            // Get source name and description from the first news item in the list.
            this.meta = {
              title: 'från ' + result[0].source.name,
              description: result[0].source.description,
            };
          } else if (category) {
            // Get category name from the first news item in the list. Categories does not have descriptions.
            this.meta = {
              title: 'om ' + result[0].categories.filter(i => i.slug === category)[0].name,
              description: '',
            };
          }
        }
      }, error => console.error(error));
    });
  }

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute) {
    this.http = http;
    this.baseUrl = baseUrl;
  }
}

interface PageMeta {
  title: string;
  description: string;
}

interface NewsItem {
  title: string;
  text: string;
  url: string;
  publishDate: string;
  image: string;
  categories: Category[];
  source: Source;
}

interface Source {
  name: string;
  description: string;
  slug: string;
}

interface Category {
  name: string;
  slug: string;
}
