import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BlogService } from '../blog.service';
import { Location } from '@angular/common';
import { Category } from '../models/category';

@Component({
  selector: 'app-new-blog',
  templateUrl: './new-blog.component.html',
  styleUrls: ['./new-blog.component.css']
})
export class NewBlogComponent {
  blogForm!: FormGroup
  categories: Category[]=[];

  constructor(private blogService:BlogService, private formBuilder: FormBuilder, private location:Location ){

  }
 
  ngOnInit(){
    this.blogService.getCategories().subscribe(x=> this.categories = x);
    this.blogForm = this.formBuilder.group(
      {
        name:['',[Validators.required]],
        category:['',[Validators.required]]
      }
    )
  }


  addNewBlog(){
    var name = this.blogForm.get('name')?.value;
    var idCategory = this.blogForm.get('category')?.value;
    console.log("category of new blog is"+ idCategory);
    this.blogService.addNewBlog(name, idCategory).subscribe(
      error=>{
          console.log(console.error(error));
        
      }
    )
    this.goBack();
  }

  goBack(): void {
    this.location.back();
  }
}