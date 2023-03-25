import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import Post from '../models/Post';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html'
})
export class PostComponent implements OnInit {
  baseUrl!: string;
  private routeSub$!: Subscription;
  post!: Post;

  constructor(private http: HttpClient,
    private route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string) {
      this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    this.routeSub$ = this.route.params.subscribe(params => {
      const postId = params['id'];
      this.http.get<Post>(this.baseUrl + `posts/${postId}`)
      .subscribe(result => {
        this.post = result;
      }, error => console.error(error));
    });
  }
}
