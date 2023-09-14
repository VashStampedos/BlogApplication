import { Component, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BlogService } from '../blog.service';
import { Article } from '../models/article';
import { LikeResponse } from '../UserClaim';
import { Like } from '../models/like';

@Component({
  selector: 'app-like',
  templateUrl: './like.component.html',
  styleUrls: ['./like.component.css']
})
export class LikeComponent {
 
  @Input() article?:Article
  likes:Like[]=[]
  isLiked?:boolean;
  
  constructor(private blogService:BlogService){
      
  }

  ngOnInit(){
    this.getArticleLikes()
    console.log("id from like init: "+ this.article?.id)
  }

  getArticleLikes(){
    //console.log("in subscribe of get comments")
    let id = this.article?.id
    if(id!=0){
      this.blogService.getLikes(id!).subscribe(
        {
          next:(x:LikeResponse)=>{
            this.likes= x.likesModel
            this.isLiked = x.isLiked;
            console.log("is liked :" + this.isLiked)
            console.log("in subscribe of get likes")
  
          },
          error:(err:Error)=>{
            console.log(err.message)
          }
        }
      )
    }
  }
  
   addNewLike(){
    let id = this.article?.id
 
    if(id!=0){
      
      this.blogService.addLike(id!).subscribe({
        next:(x:any)=>{
         
          this.getArticleLikes();
        },
        error:(err:any)=>{
          console.log(err)
        }
      })
    }
   }
}
