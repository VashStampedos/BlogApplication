import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { BlogComponent } from './blog/blog.component';
import { UserComponent } from './user/user.component';
import { ArticleComponent } from './article/article.component';
import { CommentComponent } from './comment/comment.component';
import { AuthComponent } from './auth/auth.component';
import { UserClaim } from './UserClaim';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LogOutComponent } from './log-out/log-out.component';
import { Router } from '@angular/router';
import { AuthInterceptor } from './auth-interceptor';
import { AuthGuard } from './auth-guard';
import { ArticleEditorComponent } from './article-editor/article-editor.component';
import { NewBlogComponent } from './new-blog/new-blog.component';
import { YourBlogsComponent } from './your-blogs/your-blogs.component';
import { YourArticlesComponent } from './your-articles/your-articles.component';
import { RegisterComponent } from './register/register.component';
import { LikeComponent } from './like/like.component';
import { ArticleDetailsComponent } from './article-details/article-details.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BlogComponent,
    UserComponent,
    ArticleComponent,
    CommentComponent,
    AuthComponent,
    LogOutComponent,
    ArticleEditorComponent,
    NewBlogComponent,
    YourBlogsComponent,
    YourArticlesComponent,
    RegisterComponent,
    LikeComponent,
    ArticleDetailsComponent,
    
  ],
  imports: [
    
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useFactory: function(router:Router){
        return new AuthInterceptor(router);
      },
      multi: true,
      deps:[Router]
    },
     AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
