import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavMenuComponent } from './shared/nav-menu/nav-menu.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,NavMenuComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'hotel-project';
}
