import { Component, OnInit,Input } from '@angular/core';
import { BlogService } from '../blog.service';
import { Article } from '../models/article';
import { Blog } from '../models/blog';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css']
})
//сделать авторизацию
export class ArticleComponent {
  articleTitle="";
  blog?:Blog;
  //viewArticles:Article
  articles?:Article[]=[];
  length:number=0;
  constructor(private blogService:BlogService, private route: ActivatedRoute,  private location: Location) {
        
  }

  ngOnInit(){
    this.getArticlesOfBlog();
    
  }

  getArticlesOfBlog(){
    
    const id = Number(this.route.snapshot.paramMap.get('id'));
    console.log(`id from article comp ${id}`);
    this.blogService.getBlog(id).subscribe(x=>{
      this.blog=x;
      this.articles = this.blog.articles;

    } );

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
}
