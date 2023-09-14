import { Component } from '@angular/core';
import { Blog } from '../models/blog';
import { BlogService } from '../blog.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-your-blogs',
  templateUrl: './your-blogs.component.html',
  styleUrls: ['./your-blogs.component.css']
})
export class YourBlogsComponent {

  blogs:Blog[]=[];
  constructor(private blogService:BlogService, private location:Location){

  }
  
  ngOnInit(){
    this.blogService.getUserBlogs().subscribe(x=>{this.blogs=x});
  }

  deleteBlog(id:number){
    if(id!=0)
    this.blogService.deleteBlog(id).subscribe();
    this.reload();
  }

  reload(){
    window.location.reload()
  }
}
