import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html'
})
export class CreatePostComponent implements OnInit {
  baseUrl: string;
  title: string = '';
  body: string = '';
  favouriteCount: number = 0;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private router: Router) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
  }

  savePost = () => {
    const data = {
      title: this.title,
      body: this.body,
      favoriteCount: this.favouriteCount,
    };

    this.http.post<any>(this.baseUrl + 'posts', data)
      .subscribe(_ => {
        this.router.navigate(['/home-redis']);
      });
  }
}
