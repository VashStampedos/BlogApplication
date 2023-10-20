import { HttpStatusCode } from "@angular/common/http";

export class ApiResult<T>{
    
    succeeded?:boolean;
    errors?:[];
    code?:HttpStatusCode
    data:any    
}