import { Component, Input } from '@angular/core';
import { Article } from '../models/article';
import { BlogService } from '../blog.service';
import { Comment } from '../models/comment';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent {
  commentForm!:FormGroup;
  @Input() article?:Article
  comments:Comment[]=[]
  
  constructor(private blogService:BlogService, private formBuilder: FormBuilder){
      
  }

  ngOnInit(){
    this.commentForm = this.formBuilder.group(
      {
        description:['', Validators.required],
      })
    this.getArticleComments()
    
  }

  getArticleComments(){
    //console.log("in subscribe of get comments")
    let id = this.article?.id
    if(id!=0){
      this.blogService.getComments(id!).subscribe(
        {
          next:(x:any)=>{
            this.comments=x.data
           
  
          },
        }
      )
    }
  }
  
  addNewComment(){
    let id = this.article?.id
    const description = this.commentForm.get('description')?.value;
    if(id!=0 && description.length > 0){
      
      this.blogService.addComment(id!,description).subscribe({
        next:(x:any)=>{
          let control = this.commentForm.get('description');
          control?.setValue("");
          this.getArticleComments();
        },
        error:(err:any)=>{
          console.log(err)
        }
      })
    }
  }


}
