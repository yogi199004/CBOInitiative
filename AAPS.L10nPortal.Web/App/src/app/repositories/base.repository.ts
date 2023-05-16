import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IRequest } from "../entities/request.interface";
import { TimeoutService } from "../services/timeout.service";
import { map, timeout } from "rxjs/operators";

@Injectable()
export class BaseRepository {
  constructor(
    protected http: HttpClient,
    private timeoutService: TimeoutService
  ) {}

  protected post<T>(request: IRequest): Promise<T> {
    this.updateSessionTime();

    return this.http
      .post(request.Url, request.Body)
      .pipe(map((data: any) => <T>data))
      .toPromise();
  }

  protected get<T>(url: string, options = {}): Promise<T> {
    this.updateSessionTime();

    return this.http
      .get(url, options)
      .pipe(map((data: any) => <T>data))
      .toPromise();
  }

  protected delete<T>(url: string, options = {}): Promise<T> {
    this.updateSessionTime();

    return this.http
      .post(url, options)
      .pipe(map((data: any) => <T>data))
      .toPromise();
  }

  private updateSessionTime() {
    this.timeoutService.updateSessionValidTo();
  }
}
