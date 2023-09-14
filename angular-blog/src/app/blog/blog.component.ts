import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { Blog } from '../models/blog';
import { BlogService } from '../blog.service';
import { Article } from '../models/article';
import { map } from 'rxjs';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent {
  @Input() nameBlog= "";
  blogs:Blog[]=[];
  showBlog:Blog[]=[];
  articles:Article[]=[];
  selectedBlog?:Blog;
  constructor(private blogService:BlogService) {
    
  }
  
  ngOnInit(){
    
    this.blogService.getBlogs().subscribe(x=> {this.blogs=x; this.showBlog= this.blogs});
  }
  
  searching(){
    if(this.nameBlog==""){
      
      console.log("in if");
      console.log(this.blogs);
      console.log(this.showBlog);
      console.log("name blog " + this.nameBlog);
      this.showBlog = this.blogs
    }
    else{
      this.showBlog = this.blogs.filter( x=> x.name?.includes(this.nameBlog));
      console.log("name blog " + this.nameBlog);
      console.log("in else");
    }
    
  }


  ShowArticleOfBlog(blog:Blog){
    this.selectedBlog = blog;
  }
}
