import {
  TokenContainerModel,
  AccessTokenModel,
  IdTokenModel,
  RefreshTokenModel,
} from '../../app/clients/ami-api-client';

export class TokenContainerModelExtended extends TokenContainerModel {
  accessTokenParsed: AccessTokenModel;
  idTokenParsed: IdTokenModel;
  refreshTokenParsed: RefreshTokenModel;
}
