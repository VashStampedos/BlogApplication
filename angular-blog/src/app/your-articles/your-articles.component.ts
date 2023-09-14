import { Component } from '@angular/core';
import { Blog } from '../models/blog';
import { BlogService } from '../blog.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../models/user';

@Component({
  selector: 'app-your-articles',
  templateUrl: './your-articles.component.html',
  styleUrls: ['./your-articles.component.css']
})
export class YourArticlesComponent {
  articleForm!:FormGroup;
  user!:User;
  blogs:Blog[] = [];
  blog?:Blog;
    constructor(private blogService:BlogService, private route:ActivatedRoute,  private location: Location, private formBuilder:FormBuilder){

    }

    ngOnInit(){
      this.getArticlesOfBlog();
    }

    getArticlesOfBlog(){
    
      const id = Number(this.route.snapshot.paramMap.get('id'));
      console.log(`id from article comp ${id}`);
      this.blogService.getBlog(id).subscribe(x=>{
        this.blog=x
      } );
      
    }

    deleteArticle(id:number){
      this.blogService.deleteArticle(id).subscribe();
      this.reload();
    }

    goBack(): void {
      this.location.back();
    }
    reload(){
      window.location.reload()
    }
}
