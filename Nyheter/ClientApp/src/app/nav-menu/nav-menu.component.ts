import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public sources: MenuItem[];
  public categories: MenuItem[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<MenuItem[]>(baseUrl + 'api/sources').subscribe(result => {
      this.sources = result;
    }, error => console.error(error));
  }
}

interface MenuItem {
  name: string;
  slug: string;
}
