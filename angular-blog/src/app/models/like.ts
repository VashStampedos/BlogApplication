import { Article } from "./article";
import { User } from "./user";

export class Like{
    id?:number;
    idUser?:number;
    idArticle?:number;
    user?:User;
    article?:Article;
}