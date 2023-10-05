import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BlogComponent } from './blog/blog.component';
import { ArticleComponent } from './article/article.component';
import { AuthGuard } from './auth-guard';
import { UserComponent } from './user/user.component';
import { AuthComponent } from './auth/auth.component';
import { LogOutComponent } from './log-out/log-out.component';
import { ArticleEditorComponent } from './article-editor/article-editor.component';
import { NewBlogComponent } from './new-blog/new-blog.component';
import { YourBlogsComponent } from './your-blogs/your-blogs.component';
import { YourArticlesComponent } from './your-articles/your-articles.component';
import { RegisterComponent } from './register/register.component';
import { ArticleDetailsComponent } from './article-details/article-details.component';
import { ProfileComponent } from './profile/profile.component';
import { HomeComponent } from './home/home.component';
//попытаться сделать так чтобы компоненты для которых нужен auth были скрыты
const routes: Routes = [
  { path: '', redirectTo:'/home', pathMatch:'full'},
  { path: 'home', pathMatch:'full' , component: HomeComponent},
  { path: 'articles/:id', component: ArticleComponent, pathMatch:'full'},
  { path: 'addarticle', component: ArticleEditorComponent, pathMatch:'full', canActivate:[AuthGuard]},
  { path: 'profile', component:ProfileComponent, pathMatch:'full', canActivate:[AuthGuard]},
  { path: 'user/:id', component:UserComponent, pathMatch:'full', canActivate:[AuthGuard]},
  { path: 'logout', component:LogOutComponent, pathMatch:'full',canActivate:[AuthGuard]},
  { path: 'login', component:AuthComponent},
  { path: 'register', component:RegisterComponent},
  { path: 'newblog', component:NewBlogComponent, pathMatch:"full", canActivate:[AuthGuard]},
  { path: 'yourblogs', component:YourBlogsComponent, pathMatch:"full", canActivate:[AuthGuard]},
  //не забыть поправить роутинг на мои блоги и мои артиклы
  { path: 'profile/yourarticles/:id', component:YourArticlesComponent, pathMatch:"full", canActivate:[AuthGuard]},
  { path: 'articledetails/:id', component:ArticleDetailsComponent, canActivate:[AuthGuard]}
 

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
