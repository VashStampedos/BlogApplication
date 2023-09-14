import { Article } from "./article";
import { Category } from "./category";
import { User } from "./user";


export class Blog{
    id?:number;
    name?:string;
    idUser?:number;
    idCategory?:number;
    user?: User;
    category?:Category
    articles?: Article[];
}