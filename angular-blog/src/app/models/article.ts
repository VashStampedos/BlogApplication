import { Blog } from "./blog";
import { Comment } from "./comment";
import { Like } from "./like";

export class Article{
    id?:number;
    title?:string;
    description?:string;
    photo?:string;
    idBlog?:number;
    blog?:Blog;
    likes?: Like[];
    comments?: Comment[];
}