import { Component } from '@angular/core';
import { Blog } from '../models/blog';
import { BlogService } from '../blog.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../models/user';
import { Article } from '../models/article';

@Component({
  selector: 'app-your-articles',
  templateUrl: './your-articles.component.html',
  styleUrls: ['./your-articles.component.css']
})
export class YourArticlesComponent {
  articleForm!:FormGroup;
  user!:User;
  blog?:Blog;
  articleTitle="";
  articles?:Article[]=[];
  length:number=0;

    constructor(private blogService:BlogService, private route:ActivatedRoute,  private location: Location, private formBuilder:FormBuilder){

    }

    ngOnInit(){
      this.getArticlesOfBlog();
    }

    getArticlesOfBlog(){
    
      const id = Number(this.route.snapshot.paramMap.get('id'));
      console.log(`id from article comp ${id}`);
      this.blogService.getBlog(id).subscribe(x=>{
        this.blog=x.data;
        this.articles = x.data.articles;
      } );
      
    }

    deleteArticle(id:number){
      this.blogService.deleteArticle(id).subscribe();
      this.reload();
    }

    cutDescription(desc:string){
      let description = desc;
      return  description.substring(0,100)
    }
    
    goBack(): void {
      this.location.back();
    }
    searching(){
      if(this.articleTitle==""){
        this.articles = this.blog?.articles
      }
      else{
        this.articles = this.blog?.articles?.filter( x=> x.title?.toLowerCase().includes(this.articleTitle.toLowerCase()));
        console.log("name blog " + this.articleTitle);
        console.log("in else");
      }
      
    }
    reload(){
      window.location.reload()
    }
}
