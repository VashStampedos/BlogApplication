import { Component } from '@angular/core';
import { Blog } from '../models/blog';
import { BlogService } from '../blog.service';
import { Location } from '@angular/common';
import { Category } from '../models/category';

@Component({
  selector: 'app-your-blogs',
  templateUrl: './your-blogs.component.html',
  styleUrls: ['./your-blogs.component.css']
})
export class YourBlogsComponent {
  nameBlog= "";
  blogs:Blog[]=[];
  showBlog?:Blog[]=[];
  selectedBlog?:Blog;
  categories:Category[]=[];
  selectedCategoryId:number = 0;
  inCategoryFlag:boolean = false;
  constructor(private blogService:BlogService, private location:Location){

  }
  
  ngOnInit(){
    this.blogService.getCurrentUserBlogs().subscribe(x=>{this.blogs=x; this.showBlog = this.blogs});
  }

  deleteBlog(id:number){
    if(id!=0)
    this.blogService.deleteBlog(id).subscribe();
    this.reload();
  }

  reload(){
    window.location.reload()
  }

  searching(){
    
    console.log(this.showBlog)
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

}
