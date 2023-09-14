import { Article } from "./article";
import { User } from "./user";

export class Comment{
    id?:number;
    description?:string;
    idUser?:number;
    idArticle?:number;
    user?: User;
    article?:Article;
}