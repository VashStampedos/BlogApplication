import { Component, Input } from '@angular/core';
import { BlogService } from '../blog.service';
import { Article } from '../models/article';

@Component({
  selector: 'app-article-details',
  templateUrl: './article-details.component.html',
  styleUrls: ['./article-details.component.css']
})
export class ArticleDetailsComponent {

    @Input() article?:Article;
    constructor(private blogService: BlogService){

    }
    ngOnInit(){

    }

}
