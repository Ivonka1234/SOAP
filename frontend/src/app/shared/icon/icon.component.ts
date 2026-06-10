import { Component, input } from '@angular/core';

export type IconName =
  | 'plane'
  | 'flower'
  | 'sparkle'
  | 'map'
  | 'pin'
  | 'calendar'
  | 'suitcase'
  | 'money'
  | 'tag'
  | 'calendar-days'
  | 'warning'
  | 'check'
  | 'clock'
  | 'dollar'
  | 'edit'
  | 'trash'
  | 'star'
  | 'moon'
  | 'heart';

@Component({
  selector: 'app-icon',
  standalone: true,
  templateUrl: './icon.component.html',
  styleUrl: './icon.component.scss'
})
export class IconComponent {
  name = input.required<IconName>();
  size = input(20);
}
