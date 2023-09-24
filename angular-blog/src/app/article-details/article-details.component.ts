import { Component, Input } from '@angular/core';
import { BlogService } from '../blog.service';
import { Article } from '../models/article';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-article-details',
  templateUrl: './article-details.component.html',
  styleUrls: ['./article-details.component.css']
})
export class ArticleDetailsComponent {

    article?:Article;
    constructor(private blogService: BlogService, private route: ActivatedRoute, private location: Location){

    }
    ngOnInit(){
      this.getArticlesOfBlog();
    }

    getArticlesOfBlog(){
    
      const id = Number(this.route.snapshot.paramMap.get('id'));
      this.blogService.getArticle(id).subscribe(x=>{
        this.article=x
      } );
    }
    goBack(): void {
      this.location.back();
    }
}
