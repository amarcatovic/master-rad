import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { HomeRedisComponent } from './home-redis/home-redis.component';
import { HomeMongoComponent } from './home-mongo/home-mongo.component';
import { HomeSqlServerComponent } from './home-sql-server/home-sql-server.component';
import { HomeInMemoryComponent } from './home-in-memory/home-in-memory.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CreatePostComponent } from './create-post/create-post.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    HomeRedisComponent,
    HomeMongoComponent,
    HomeSqlServerComponent,
    HomeInMemoryComponent,
    CounterComponent,
    FetchDataComponent,
    CreatePostComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'redis', component: HomeRedisComponent, pathMatch: 'full' },
      { path: 'mongo', component: HomeMongoComponent, pathMatch: 'full' },
      { path: 'sql-cache', component: HomeSqlServerComponent, pathMatch: 'full' },
      { path: 'in-memory', component: HomeInMemoryComponent, pathMatch: 'full' },
      { path: 'new', component: CreatePostComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ]),
    BrowserAnimationsModule,
    MatTableModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
