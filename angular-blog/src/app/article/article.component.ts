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
  blog?:Blog;
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
      this.blog=x
    } );
  }

  
  goBack(): void {
    this.location.back();
  }
}
