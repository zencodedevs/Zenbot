/** core imports */
import { Component, OnInit } from "@angular/core";

/** internal imports */
import { AuthorizeService } from "../../api-authorization/authorize.service";

import { faCopy } from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: "app-token-component",
  templateUrl: "./token.component.html",
})
export class TokenComponent implements OnInit {

  /**
   * Token  of token component
   */
  token: string;

  /**
   * Determines whether error is
   */
  isError: boolean;

  /**
   * Determines whether copied is
   */
  isCopied: boolean;

  /**
   * Fa copy of token component
   */
  faCopy = faCopy;

  /**
   * Creates an instance of token component.
   * @param authorizeService 
   */
  constructor(private authorizeService: AuthorizeService) { }

  /**
   * on init
   */
  ngOnInit(): void {
    this.isCopied = false;
    this.authorizeService.getAccessToken().subscribe(
      (t) => {
        this.token = "Bearer " + t;
        this.isError = false;
      },
      (err) => {
        this.isError = true;
      }
    );
  }

  /**
   * Copys to clipboard
   */
  copyToClipboard(): void {
    const selBox = document.createElement("textarea");
    selBox.style.position = "fixed";
    selBox.style.left = "0";
    selBox.style.top = "0";
    selBox.style.opacity = "0";
    selBox.value = this.token;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand("copy");
    document.body.removeChild(selBox);
    this.isCopied = true;
  }
}
