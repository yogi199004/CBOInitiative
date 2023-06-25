import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IUser } from '../../entities/user.interface';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})

export class HeaderComponent implements OnInit {
  userDetails: IUser;
  toggleDropDown: boolean = false;
  userNameDisplay: string = '';

  constructor(
    private userService: UserService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.userService
      .getCurrentUserAsync()
      .then(data => {
        
        this.userDetails = data;

      
      });

    //const userName = "Yogesh, Dubey";
    //this.userNameDisplay = "Yogesh Dubey";
  }

  navigateToDashboard(): void {
    this.router.navigate([""]);
  }

  openDropdDown(): void {
    this.toggleDropDown = !this.toggleDropDown;
  }

  logout(): void {
    window.location.href = "/Home/Logout";
  }
}
