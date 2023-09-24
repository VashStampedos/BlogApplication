import { Injectable } from '@angular/core';
import { Blog } from './models/blog';
import { HttpClient } from '@angular/common/http'
import { Observable, map, of } from 'rxjs';
import { Article } from './models/article';
import { AuthService } from './auth.service';
import { Comment } from './models/comment';
import { Like } from './models/like';
import { LikeResponse } from './UserClaim';
import { Category } from './models/category';
import { User } from './models/user';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  constructor(private http:HttpClient) {
    
  }
  
  getCategories():Observable<Category[]>{
    return this.http.get<Category[]>("https://localhost:7018/Blog/Categories");
  }
  getBlogs():Observable<Blog[]>{
    return this.http.get<Blog[]>("https://localhost:7018/Blog/Blogs");
  }
  getCurrentUserBlogs():Observable<Blog[]>{
    return this.http.get<Blog[]>("https://localhost:7018/Blog/GetCurrentUserBlogs");
  }
  getUserBlogs(id:number):Observable<Blog[]>{
    return this.http.get<Blog[]>(`https://localhost:7018/Blog/GetUserBlogs?id=${id}`);
  }
  getBlog(id:number):Observable<Blog>{
    
    const blog = this.http.get<Blog>(`https://localhost:7018/Blog/GetBlog?id=${id}`);
    return blog;
  }
  addNewBlog(name:string, idCategory:number){
    return this.http.post("https://localhost:7018/Blog/AddNewBlog",{name, idCategory});
  }
  deleteBlog(id:number){
    return this.http.delete(`https://localhost:7018/Blog/DeleteBlog?id=${id}`)
  }
  getComments(id:number):Observable<Comment[]>{
    
    return this.http.get<Comment[]>(`https://localhost:7018/Blog/GetComments?id=${id}`);
  }
  addComment(idArticle:number, description:string){
    return this.http.post
    ("https://localhost:7018/Blog/AddNewComment",{idArticle:idArticle,description:description})
  }
  getLikes(id:number):Observable<LikeResponse>{
    console.log("id from getLikes: "+ id)
    return this.http.get<LikeResponse>(`https://localhost:7018/Blog/GetLikes?id=${id}`);
  }
  addLike(idArticle:number){
    return this.http.post
    ("https://localhost:7018/Blog/AddLike",{idArticle:idArticle})
  }
  getArticles():Observable<Article[]>{
    return this.http.get<Article[]>("https://localhost:7018/Blog/Articles");
  }
  getArticle(id:number):Observable<Article>{
    return this.http.get<Article>(`https://localhost:7018/Blog/GetArticle?id=${id}`);
  }
  addArticle(title:string, description:string, photo:File, idblog:number){
    
    const formData = new FormData();
    
    formData.append('title', title)
    formData.append("description", description)
    formData.append("photo", photo)
    formData.append("idblog", ""+idblog)
    formData.forEach(x=> console.log(x.valueOf()))
    // console.log("formData")
    // console.log(formData)
    return this.http.post
    ("https://localhost:7018/Blog/AddNewArticle",formData)
  }
  deleteArticle(id:number){
    console.log("del from service id "+ id)
    return this.http.delete(`https://localhost:7018/Blog/DeleteArticle?id=${id}`)
  }
}
