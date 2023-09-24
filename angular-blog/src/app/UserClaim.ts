import { Like } from "./models/like";
import { User } from "./models/user";

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
export interface UserResponse{
    userModel:User;
    isSubscribe:boolean;
    
}