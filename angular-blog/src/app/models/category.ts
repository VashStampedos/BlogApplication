import { Blog } from "./blog";


export class Category{
    id?:number;
    name?:string;
    blogs?: Blog[];
}