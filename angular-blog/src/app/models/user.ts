import { Like } from "./like";
import { Comment } from "./comment";
import { Subscribe } from "./subscribe";
import { Blog } from "./blog";

export class User{
    id?:number; 
    email?:string
    passwordHash?:string
    blogs?:Blog[]; 
    likes?:Like[]; 
    comments?:Comment[];
    subscribes?:Subscribe[];
}