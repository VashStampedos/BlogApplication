import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { Blog } from '../models/blog';
import { BlogService } from '../blog.service';
import { Article } from '../models/article';
import { map } from 'rxjs';
import { Category } from '../models/category';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent {
  nameBlog= "";
  @Input() blogs?:Blog[];
  @Input() showBlog?:Blog[]=[];
  selectedBlog?:Blog;
  categories:Category[]=[];
  selectedCategoryId:number = 0;
  inCategoryFlag:boolean = false;

  constructor(private blogService:BlogService) {
    
  }
  
  ngOnInit(){
    
    //this.blogService.getBlogs().subscribe(x=> {this.blogs=x; this.showBlog= this.blogs});
    this.blogService.getCategories().subscribe(x=>{this.categories = x;})
    //this.showBlog = this.blogs
  }
  
  
  searching(){
    
    console.log(this.blogs)
    if(!this.inCategoryFlag){

      if(this.nameBlog==""){
        this.showBlog = this.blogs
      }
      else{
        this.showBlog = this.blogs?.filter( x=> x.name?.toLowerCase().includes(this.nameBlog.toLowerCase()));
        console.log("name blog " + this.nameBlog);
        console.log("in else");
      }
    }else{
      if(this.nameBlog==""){
        this.showBlog = this.blogs?.filter(x=>x.idCategory == this.selectedCategoryId)
      }
      else{
        this.showBlog = this.blogs?.filter( x=> x.name?.toLowerCase().includes(this.nameBlog.toLowerCase()) && x.idCategory == this.selectedCategoryId);
        console.log("name blog " + this.nameBlog);
        console.log("in else");
      }
    }
  }

  SortByCategory(){
    console.log(this.selectedCategoryId)
    if(this.selectedCategoryId == 0){
      this.inCategoryFlag = false
      this.showBlog = this.blogs;

    }
    else{
      this.inCategoryFlag = true;
      this.showBlog = this.blogs?.filter(x=> x.category?.id == this.selectedCategoryId)

    }
  }


  ShowArticleOfBlog(blog:Blog){
    this.selectedBlog = blog;
  }
}
