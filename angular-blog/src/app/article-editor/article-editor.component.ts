import { Component, numberAttribute } from '@angular/core';
import { BlogService } from '../blog.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { User } from '../models/user';
import { Blog } from '../models/blog';

@Component({
  selector: 'app-article-editor',
  templateUrl: './article-editor.component.html',
  styleUrls: ['./article-editor.component.css']
})
export class ArticleEditorComponent {
  articleForm!:FormGroup;
  user!:User;
  blogs:Blog[] = [];
  blog?:Blog;
  photo?:File;
  
    constructor(private blogService:BlogService, private route:ActivatedRoute,  private location: Location, private formBuilder:FormBuilder){

    }

    ngOnInit(){
        this.blogService.getCurrentUserBlogs().subscribe(x=>{
          this.blogs= x
        } )

     
      this.articleForm = this.formBuilder.group(
        {
          title:['', [Validators.required]],
          description:['', Validators.required],
          photo:['', Validators.required],

        }
      )
      
    }

    addNewArticle(){
      if(!this.articleForm.valid){
        console.log("new article invalid")
        return;
      }
      const title = this.articleForm.get('title')?.value;
      const description = this.articleForm.get('description')?.value;
      //const photo = this.articleForm.get('photo')?.value;
      const idBlog = this.blog?.id!;
      
      console.log("new art" + title,description,this.photo,idBlog)
      this.blogService.addArticle(title, description, this.photo!, idBlog).subscribe(
        error=>{
            console.log(error)
        }
      );
      this.goBack();
    }

    onFileSelected(event:any){
      if(event.target.files.length>0){
        this.photo = event.target.files[0];
        console.log(event.target.files[0])
      }
    }

    onClickBlog(blog:Blog){
      console.log("blog selected")
      this.blog=blog;
    }
    log(){
      console.log("click")
    }
    goBack(): void {
      this.location.back();
    }
}
