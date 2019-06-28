import { ContainerModelExtended } from './container-extended.model';
import {
  ProcessResultModel,
  AxisContainerModelOfString,
  PositionAxisContainerModelOfString
} from '../../app/clients/ami-api-client';

export interface ProcessResultModelExtended extends ProcessResultModel {
  combinedGifExtended: ContainerModelExtended;
  showCombinedGif: boolean;
  xAxisGif: AxisContainerModelOfString & ContainerModelExtended;
  yAxisGif: AxisContainerModelOfString & ContainerModelExtended;
  zAxisGif: AxisContainerModelOfString & ContainerModelExtended;
  xAxisImages: PositionAxisContainerModelOfString & ContainerModelExtended[];
  yAxisImages: PositionAxisContainerModelOfString & ContainerModelExtended[];
  zAxisImages: PositionAxisContainerModelOfString & ContainerModelExtended[];
}
