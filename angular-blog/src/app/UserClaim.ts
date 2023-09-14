import { Like } from "./models/like";

export interface UserClaim{
    type: string;
    value: string;
}
export interface Response{
    isSuccess:boolean;
    message:string;
    claims: UserClaim[];
}
export interface LikeResponse{
    likesModel:Like[];
    isLiked:boolean;
    
}