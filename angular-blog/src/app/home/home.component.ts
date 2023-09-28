import { Component, Input } from '@angular/core';
import { BlogService } from '../blog.service';
import { Blog } from '../models/blog';
import { Category } from '../models/category';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  nameBlog= "";
  blogs:Blog[]=[];
  selectedBlog?:Blog;
  
  constructor(private blogService:BlogService) {
    
  }
  
  ngOnInit(){
    
    this.blogService.getBlogs().subscribe(x=> {this.blogs=x; console.log("asdasdsad")});
    
  }
  
  


  ShowArticleOfBlog(blog:Blog){
    this.selectedBlog = blog;
  }
}
