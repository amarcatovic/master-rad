import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import Post from '../models/Post';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  baseUrl!: string;
  posts!: Post[];
  displayedColumns: string[] = ['title', 'created', 'viewCount', 'favouriteCount',];
  loading!: boolean | null;
  searchTerm: string = '';

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    this.loading = null;
    // this.loadData();
  }

  loadData = () => {
    this.loading = true;
    this.http.get<Post[]>(this.baseUrl + 'posts/top-favourite')
      .subscribe(result => {
        this.posts = result;
        this.loading = false;
      }, error => console.error(error));
  }

  searchPosts = () => {
    if (this.searchTerm === '') {
      this.displayedColumns = ['title', 'created', 'viewCount', 'favouriteCount'];
      return;
    }

    this.displayedColumns = ['title'];
    this.posts = [];
    this.loading = true;
    this.http.get<Post[]>(this.baseUrl + `posts/search?term=${this.searchTerm}`)
      .subscribe(result => {
        this.posts = result;
        this.loading = false;
      }, error => console.error(error));
  }
}
